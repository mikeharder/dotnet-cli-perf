package ClassLib021;

public class Class001 {
    public static String property() {
        return "ClassLib021" + " " + ClassLib007.Class001.property() + " " + ClassLib017.Class001.property();
    }
}
