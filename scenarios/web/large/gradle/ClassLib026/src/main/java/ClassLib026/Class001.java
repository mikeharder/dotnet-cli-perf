package ClassLib026;

public class Class001 {
    public static String property() {
        return "ClassLib026" + " " + ClassLib011.Class001.property() + " " + ClassLib014.Class001.property();
    }
}
