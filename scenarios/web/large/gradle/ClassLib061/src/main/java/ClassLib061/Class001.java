package ClassLib061;

public class Class001 {
    public static String property() {
        return "ClassLib061" + " " + ClassLib050.Class001.property() + " " + ClassLib025.Class001.property() + " " + ClassLib026.Class001.property();
    }
}
