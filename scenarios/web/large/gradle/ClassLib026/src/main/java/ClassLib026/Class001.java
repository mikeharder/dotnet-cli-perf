package ClassLib026;

public class Class001 {
    public static String property() {
        return "ClassLib026" + " " + ClassLib005.Class001.property() + " " + ClassLib011.Class001.property();
    }
}
